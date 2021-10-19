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
        /// The background <see cref="Color"/>
        /// defined as 0x8A, 0x9D, 0xC7
        /// </summary>
        public static readonly Color BackgroundColor = Color.FromArgb(0x8A, 0x9D, 0xC7);

        /// <summary>
        /// The green <see cref="Color"/>
        /// defined as 0xB8, 0xF2, 0x94
        /// </summary>
        public static readonly Color Green = Color.FromArgb(189, 220, 4);

        /// <summary>
        /// The black <see cref="Color"/>
        /// defined as 0x2F, 0x4F, 0x4F
        /// </summary>
        public static readonly Color Black = Color.DarkSlateGray;

        /// <summary>
        /// The text light color <see cref="Color"/>.
        /// See <see cref="Color.Transparent"/>
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;

        /// <summary>
        /// The light text color <see cref="Color"/>
        /// defined as 0xFC, 0xEE, 0xDF
        /// </summary>
        public static readonly Color TextColorLight = Color.FromArgb(0xFC, 0xEE, 0xDF);

        /// <summary>
        /// The dark text color <see cref="Color"/>
        /// defined as 0x2F, 0x4F, 0x4F
        /// </summary>
        public static readonly Color TextColorDark = Color.DarkSlateGray;

        public static readonly Color TextColor = Color.FromArgb(51, 51, 51);

        public static readonly Color Grey = Color.FromArgb(200, 200, 200);

        /// <summary>
        /// The red <see cref="Color"/>
        /// defined as 0xD2, 0x04, 0x2D
        /// </summary>
        public static readonly Color Red = Color.FromArgb(0xD2, 0x04, 0x2D);

        /// <summary>
        /// The light blue <see cref="Color"/>
        /// defined as 0xDE, 0xE7, 0xF2
        /// </summary>
        public static readonly Color LightBlue = Color.FromArgb(0xDE, 0xE7, 0xF2);

        /// <summary>
        /// The light yellow <see cref="Color"/>
        /// defined as 0xFF, 0xFe, 0xED
        /// </summary>
        public static readonly Color LightYellow = Color.FromArgb(0xFF, 0xFE, 0xED);

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