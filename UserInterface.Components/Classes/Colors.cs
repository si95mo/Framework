using System.Drawing;

namespace UserInterface.Controls
{
    /// <summary>
    /// Provides <see cref="Color"/> constants
    /// to use when creating an user interface
    /// </summary>
    public class Colors
    {
        /// <summary>
        /// The background <see cref="Color"/> defined as 0xFA, 0xFA, 0xFA
        /// </summary>
        public static readonly Color BackgroundColor = Color.FromArgb(0xFA, 0xFA, 0xFA);

        /// <summary>
        /// The green <see cref="Color"/> defined as 0xC7, 0xD1, 0x17
        /// </summary>
        public static readonly Color Green = Color.FromArgb(0xC7, 0xD1, 0x17);

        /// <summary>
        /// The black <see cref="Color"/> defined as 0x2F, 0x4F, 0x4F
        /// </summary>
        public static readonly Color Black = Color.DarkSlateGray;

        /// <summary>
        /// The text light <see cref="Color"/>. See <see cref="Color.Transparent"/>
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;

        /// <summary>
        /// The light text <see cref="Color"/> defined as 0xFC, 0xEE, 0xDF
        /// </summary>
        public static readonly Color TextColorLight = Color.FromArgb(0xFC, 0xEE, 0xDF);

        /// <summary>
        /// The dark text <see cref="Color"/> defined as 0x2F, 0x4F, 0x4F
        /// </summary>
        public static readonly Color TextColorDark = Color.DarkSlateGray;

        /// <summary>
        /// The text <see cref="Color"/> defined as 0x33, 0x33, 0x33
        /// </summary>
        public static readonly Color TextColor = Color.FromArgb(0x33, 0x33, 0x33);

        /// <summary>
        /// The gray <see cref="Color"/> defined as 0xC8, 0xC8, 0xC8
        /// </summary>
        public static readonly Color Grey = Color.FromArgb(0xC8, 0xC8, 0xC8);

        /// <summary>
        /// The red <see cref="Color"/> defined as 0xD2, 0x04, 0x2D
        /// </summary>
        public static readonly Color Red = Color.FromArgb(0xD2, 0x04, 0x2D);

        /// <summary>
        /// The light blue <see cref="Color"/> defined as 0xDE, 0xE7, 0xF2
        /// </summary>
        public static readonly Color LightBlue = Color.FromArgb(0xDE, 0xE7, 0xF2);

        /// <summary>
        /// The light yellow <see cref="Color"/> defined as 0xFF, 0xFe, 0xED
        /// </summary>
        public static readonly Color LightYellow = Color.FromArgb(0xFF, 0xFE, 0xED);

        /// <summary>
        /// The yellow <see cref="Color"/> defined as 0xFE, 0xD0, 0x00
        /// </summary>
        public static readonly Color Yellow = Color.FromArgb(0xFE, 0xD0, 0x00);

        /// <summary>
        /// Define the color for success messages
        /// </summary>
        public static readonly Color Success = Color.SeaGreen;

        /// <summary>
        /// Define the color for info messages
        /// </summary>
        public static readonly Color Info = Color.RoyalBlue;

        /// <summary>
        /// Define the color for warning messages
        /// </summary>
        public static readonly Color Warning = Color.DarkOrange;

        /// <summary>
        /// Define the color for error messages
        /// </summary>
        public static readonly Color Error = Color.DarkRed;
    }
}