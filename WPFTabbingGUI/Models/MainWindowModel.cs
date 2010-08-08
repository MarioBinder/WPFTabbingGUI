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
        string _pageDir = ConfigurationManager.AppSettings.Get("ROOTDIR");
        string _configPath = ConfigurationManager.AppSettings.Get("CONFIGPATH");
        public string Message { get; set; }
        public double ApplicationHeight { get; set; }
        public double ApplicationWidth { get; set; }
        #endregion

        public MainWindowModel()
        {
            _config = XDocument.Load(_configPath);
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
