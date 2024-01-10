using Diagnostic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms;
using UserInterface.Controls;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a control that will show a <see langword="dynamic"/> json deserialized <see cref="object"/>
    /// </summary>
    public partial class DynamicJsonPanel : UserControl
    {
        /// <summary>
        /// The json text to visualize
        /// </summary>
        public string JsonText
        {
            get
            {
                if (!string.IsNullOrEmpty(jsonText))
                {
                    ExpandoObject deserializedJson = JsonConvert.DeserializeObject<ExpandoObject>(jsonText); // Deserialize the json
                    ExpandoObject obj = new ExpandoObject(); // And create a new ExpandoObject

                    foreach (KeyValuePair<string, Control> item in values) // Iterate through the created user controls
                    {
                        // Change the value each user control text property to the actual runtime one, retrieved from the deserialized json
                        object castedValue = Convert.ChangeType(item.Value.Text, (deserializedJson as IDictionary<string, object>)[item.Key].GetType());
                        AddProperty(obj, item.Key, castedValue); // The populate the new ExpandoObject with the actual casted value
                    }

                    // Serialize the newly created ExpandoObject
                    string json = JsonConvert.SerializeObject(obj);
                    return json; // And then return the serialization result
                }
                else
                {
                    return string.Empty;
                }
            }
            set => jsonText = value;
        }

        /// <summary>
        /// Add a new property to and <see cref="ExpandoObject"/>
        /// </summary>
        /// <param name="expandoObject">The <see cref="ExpandoObject"/> to populate</param>
        /// <param name="propertyName">The name of the <paramref name="expandoObject"/> to add or change value</param>
        /// <param name="propertyValue">The value associated with the <paramref name="expandoObject"/> <paramref name="propertyName"/></param>
        private void AddProperty(ExpandoObject expandoObject, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            IDictionary<string, object> expandoDictionary = expandoObject;
            if (expandoDictionary.ContainsKey(propertyName))
            {
                expandoDictionary[propertyName] = propertyValue;
            }
            else
            {
                expandoDictionary.Add(propertyName, propertyValue);
            }
        }

        private readonly Dictionary<string, Control> values;
        private string jsonText;

        /// <summary>
        /// Create a new instance of <see cref="DynamicJsonPanel"/>
        /// </summary>
        public DynamicJsonPanel()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Inherit;

            values = new Dictionary<string, Control>();
        }

        /// <summary>
        /// Create a new instance of <see cref="DynamicJsonPanel"/>
        /// </summary>
        /// <param name="json">The json contents</param>
        public DynamicJsonPanel(string json, bool controlsEnabled) : this()
        {
            UpdateText(json, controlsEnabled);
        }

        /// <summary>
        /// Update the <see cref="DynamicJsonPanel"/> control
        /// </summary>
        /// <param name="json"></param>
        public void UpdateText(string json, bool controlsEnabled)
        {
            if (!InvokeRequired)
            {
                pnlContainer.Controls.Clear();
                jsonText = json;

                ExpandoObject deserializedJson;
                try
                {
                    deserializedJson = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

                    SetDeserializedJson(controlsEnabled, deserializedJson);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Exception when trying to deserialize a json, trying with an {nameof(IEnumerable)}. Exception message is \"{ex.Message}\"");

                    dynamic obj = JsonConvert.DeserializeObject<List<ExpandoObject>>(json, new ExpandoObjectConverter());
                    foreach(ExpandoObject deserialized in (obj as IEnumerable<ExpandoObject>))
                    {
                        SetDeserializedJson(controlsEnabled, deserialized);
                    }
                }
            }
            else
            {
                BeginInvoke(new Action(() => UpdateText(json, controlsEnabled)));
            }
        }

        private void SetDeserializedJson(bool controlsEnabled, ExpandoObject deserializedJson)
        {
            if (deserializedJson != null)
            {
                values.Clear();
                pnlContainer.Controls.Clear();
                Controls.Clear();
                Controls.Add(pnlContainer);

                Control control;
                int offset = 0, factor, numberOfResizes = 0;
                foreach (KeyValuePair<string, object> property in deserializedJson)
                {
                    if (property.Value as IEnumerable<object> != null) // Array
                    {
                        object[] array = (property.Value as IEnumerable<object>).ToArray();
                        control = new ArrayViewControl(array)
                        {
                            Location = new Point(0, 32 * offset),
                            Size = new Size(pnlContainer.Width, 32)
                        };

                        factor = 0; // No additional space
                    }
                    else if (property.Value is bool value) // Boolean
                    {
                        control = new LedControl()
                        {
                            On = value,
                            Location = new Point((pnlContainer.Width - 32) / 2, 32 * offset),
                            Size = new Size(32, 32)
                        };

                        factor = 0; // No additional space
                    }
                    else if (property.Value is ExpandoObject expandoObject) // An object
                    {
                        string objectAsJson = JsonConvert.SerializeObject(expandoObject);
                        control = new DynamicJsonPanel(objectAsJson, controlsEnabled)
                        {
                            Location = new Point(0, 32 * offset),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        factor = control.Controls.Count - 2; // Do not count border style

                        (control as DynamicJsonPanel).ResizeControls();
                        numberOfResizes++;
                    }
                    else // Remaining basic types. Numerics, strings, enums, ... (except bool)
                    {
                        control = new TextControl()
                        {
                            Text = property.Value.ToString(),
                            TextAlign = HorizontalAlignment.Center,
                            Enabled = controlsEnabled,
                            Location = new Point(0, 32 * offset),
                            Size = new Size(pnlContainer.Width, 32)
                        };

                        factor = 0;
                    }

                    offset++; // Normal increment
                    offset += factor; // Increment for additional space

                    pnlContainer.Controls.Add(control);
                    values.Add(property.Key, control);

                    AddLabel(control, property.Key);
                }

                pnlContainer.Size = new Size(pnlContainer.Width, 32 * offset);
                Height = pnlContainer.Height;
            }
        }

        /// <summary>
        /// Add a new <see cref="LabelControl"/> to <see langword=""="this"/>
        /// </summary>
        /// <param name="relatedControl">The related <see cref="Control"/> (the on the same row)</param>
        /// <param name="text">The <see cref="LabelControl"/> text</param>
        private void AddLabel(Control relatedControl, string text)
        {
            LabelControl label = new LabelControl()
            {
                Text = text,
                Font = new Font("Lucida Sans Unicode", 10f),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Size = new Size(Width - pnlContainer.Width, 32),
                Location = new Point(0, relatedControl.Location.Y)
            };
            Controls.Add(label);
        }

        /// <summary>
        /// Resize all the controls
        /// </summary>
        private void ResizeControls()
        {
            int decrement = Width - pnlContainer.Width; // Positive
            foreach (Control ctrl in Controls)
            {
                if (ctrl is Panel)
                {
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        subCtrl.Width -= decrement; // Negative, subtract
                        subCtrl.Invalidate();
                    }
                }
            }
        }
    }
}