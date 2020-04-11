using System;
using System.Collections.Generic;
using System.Text;

namespace WebExpressAgent.Model
{
    public class Plugin
    {
        /// <summary>
        /// Liefert oder setzt den Namen
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Liefert oder setzt die BasisUrl
        /// </summary>
        public string BasisUrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die IconUrl
        /// </summary>
        public string IconUrl { get; set; }
    }
}
