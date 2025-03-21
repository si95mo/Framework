﻿using Core;
using Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace Hardware.Opc.Ua
{
    /// <summary>
    /// Implement a resource that perform communication
    /// with the hardware through the OPC UA protocol
    /// </summary>
    public class OpcUaResource : Resource
    {
        private string applicationName;
        private string applicationUri;
        private string serverAddress;

        private ApplicationDescription applicationDescription;
        private UaTcpSessionChannel channel;

        private Dictionary<string, ReadRequest> readRequests;
        private Dictionary<string, WriteRequest> writeRequests;

        public override bool IsOpen => channel.State == CommunicationState.Opened;

        /// <summary>
        /// Create a new instance of <see cref="OpcUaResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="serverAddress">The server address</param>
        public OpcUaResource(string code, string serverAddress) : base(code)
        {
            this.serverAddress = serverAddress;

            applicationName = $"UaClient.{code}";
            applicationUri = $"urn:{System.Net.Dns.GetHostName()}:{code}";

            applicationDescription = new ApplicationDescription
            {
                ApplicationName = applicationName,
                ApplicationUri = applicationUri,
                ApplicationType = ApplicationType.Client
            };

            channel = new UaTcpSessionChannel(
                applicationDescription,
                null,
                new AnonymousIdentity(),
                serverAddress,
                SecurityPolicyUris.None
            );

            readRequests = new Dictionary<string, ReadRequest>();
            writeRequests = new Dictionary<string, WriteRequest>();

            channels.ItemAdded += Channels_ItemAdded;
            channels.ItemRemoved += Channels_ItemRemoved;
        }

        private void Channels_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            if (e.Item is OpcUaAnalogInput || e.Item is OpcUaDigitalInput)
                readRequests.Add(e.Item.Code, new ReadRequest());
            else
                writeRequests.Add(e.Item.Code, new WriteRequest());
        }

        private void Channels_ItemRemoved(object sender, BagChangedEventArgs<IProperty> e)
        {
            if (e.Item is OpcUaAnalogInput || e.Item is OpcUaDigitalInput)
                readRequests.Remove(e.Item.Code);
            else
                writeRequests.Remove(e.Item.Code);
        }

        /// <summary>
        /// Receive a value through the <see cref="OpcUaResource"/>
        /// </summary>
        /// <param name="code">The <see cref="IOpcUaChannel"/> code</param>
        internal async Task Receive(string code)
        {
            if (status.Value == ResourceStatus.Executing)
            {
                try
                {
                    IOpcUaChannel ch = channels.Get(code) as IOpcUaChannel;
                    ReadValueId rv = new ReadValueId
                    {
                        NodeId = NodeId.Parse(ch.NamespaceConfiguration),
                        AttributeId = AttributeIds.Value
                    };
                    readRequests[code].NodesToRead = new[] { rv };

                    ReadResponse readResult = await channel.ReadAsync(readRequests[code]);

                    if (ch is OpcUaAnalogInput) // Analog channel
                        (ch as OpcUaAnalogInput).Value = (double)readResult.Results[0].Value;
                    else // Digital channel
                        (ch as OpcUaDigitalInput).Value = (bool)readResult.Results[0].Value;
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Send a value through the <see cref="OpcUaResource"/>
        /// </summary>
        /// <param name="code">The <see cref="IOpcUaChannel"/> code</param>
        internal async Task Send(string code)
        {
            if (status.Value == ResourceStatus.Executing)
            {
                try
                {
                    IOpcUaChannel ch = channels.Get(code) as IOpcUaChannel;
                    DataValue value;
                    if (ch is OpcUaAnalogOutput)
                        value = new DataValue((ch as OpcUaAnalogOutput).Value);
                    else
                        value = new DataValue((ch as OpcUaDigitalOutput).Value);

                    WriteValue wv = new WriteValue
                    {
                        NodeId = NodeId.Parse(ch.NamespaceConfiguration),
                        AttributeId = AttributeIds.Value,
                        Value = value
                    };
                    writeRequests[code].NodesToWrite = new[] { wv };

                    WriteResponse writeResult = await channel.WriteAsync(writeRequests[code]);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override async Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            try
            {
                await channel.OpenAsync();

                if (channel.State == CommunicationState.Opened)
                    Status.Value = ResourceStatus.Executing;
                else
                {
                    Status.Value = ResourceStatus.Failure;
                    failure = new Failure($"Unable to connect to {serverAddress}");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override async void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            try
            {
                await channel.CloseAsync();

                if (channel.State == CommunicationState.Closed)
                    Status.Value = ResourceStatus.Stopped;
                else
                {
                    Status.Value = ResourceStatus.Failure;
                    failure = new Failure($"Unable to disconnect to {serverAddress}");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}