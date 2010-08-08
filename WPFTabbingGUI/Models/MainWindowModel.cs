/*
 * Copyright (C) 2010, Mario Priebe <mp@biggle.de>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the project nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows.Markup;
using System.IO;
using System.Configuration;
using System.Windows.Media.Imaging;
using System;


namespace WPFTabbingGUI.Models
{
    public class MainWindowModel
    {
        #region fields
        XDocument _config = new XDocument();
        string _pageDir;
        string _configPath;
        public string Message { get; set; }
        public double ApplicationHeight { get; set; }
        public double ApplicationWidth { get; set; }
        #endregion

        public MainWindowModel()
        {
            try
            {
                _pageDir = ConfigurationManager.AppSettings.Get("ROOTDIR");
                _configPath = ConfigurationManager.AppSettings.Get("CONFIGPATH");
                _config = XDocument.Load(_configPath);
            }
            catch (Exception ex)
            {
                Message = "ConfigLoad: " + ex.Message;
            }
        }

        #region PublicMethods
        /// <summary>
        /// gets the pages as tabitems from configuration
        /// </summary>
        /// <returns></returns>
        public IList<TabItem> GetTabItems()
        {
            IList<TabItem> tabItems = new List<TabItem>();
            try
            {
                var nodes = GetNodes("TabItems");
                var items = from a in nodes.Descendants("TabItem")
                            select new
                            {
                                name = a.Attribute("Name").Value,
                                fileName = a.Attribute("FileName").Value,
                                icon = a.Attribute("Icon").Value,
                                iconHeight = a.Attribute("IconHeight").Value,
                                iconWidth = a.Attribute("IconWidth").Value
                            };

                foreach (var item in items)
                {
                    StreamReader sr = new StreamReader(_pageDir + item.fileName);
                    var content = XamlReader.Load(sr.BaseStream);
                    var image = new Image { Source = GetImageSource(item.icon), Width = Convert.ToDouble(item.iconWidth), Height = Convert.ToDouble(item.iconHeight) };
                    tabItems.Add(new TabItem { Name = item.name, Content = content, Header = image });
                    sr.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                Message = "GetTabItems: " + ex.Message;
            }

            return tabItems;
        }



        /// <summary>
        /// get the value from configuration to set the TabstripReplacement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Dock SetConfiguration()
        {
            var dock = new Dock();
            try
            {
                var nodes = GetNodes("Config");
                var items = from a in nodes.Descendants("TabStripPlacementKey")
                            select new
                            {
                                key = a.Attribute("Key").Value
                            };

                //adjust the navigation items
                foreach (var item in items)
                {
                    switch (item.key)
                    {
                        case "Left": dock = Dock.Left; break;
                        case "Right": dock = Dock.Right; break;
                        case "Top": dock = Dock.Top; break;
                        case "Bottom": dock = Dock.Bottom; break;
                        default: dock = Dock.Left; break;
                    }
                }

                //fill the application properties
                var appItems = from a in nodes.Descendants("Application")
                               select new
                               {
                                   height = a.Attribute("Height").Value,
                                   width = a.Attribute("Width").Value,
                               };

                foreach (var item in appItems)
                {
                    ApplicationHeight = Convert.ToDouble(item.height);
                    ApplicationWidth = Convert.ToDouble(item.width);
                }
            }
            catch (System.Exception ex)
            {
                Message = "SetConfiguration: " + ex.Message;
            }
            return dock;
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// private method to resolve the nodes from xml
        /// </summary>
        /// <param name="descendant"></param>
        /// <returns></returns>
        private IEnumerable<XElement> GetNodes(string descendant)
        {
            return (from n in _config.Root.Descendants(descendant)
                    select n).ToList();
        }

        /// <summary>
        /// convert the imagepath to an bitmapimage
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private BitmapImage GetImageSource(string imagePath)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.CacheOption = BitmapCacheOption.OnLoad;
            logo.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            logo.UriSource = new Uri(_pageDir + imagePath, UriKind.Relative);
            logo.EndInit();
            return logo;
        }
        #endregion
    }
}
