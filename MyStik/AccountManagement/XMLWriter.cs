using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyStik
{
    class XMLUserDBManipulator
    {
        public void AddItem(string cardKey, string name, string lastname, string username, string password, string XMLFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("userdatabase.xml");




            XmlElement newUser = doc.CreateElement("user");

            newUser.SetAttribute("cardkey", cardKey);

            XmlElement newUserName = doc.CreateElement("username");
            newUserName.InnerText = username;
            newUser.AppendChild(newUserName);

            XmlElement newPassword = doc.CreateElement("password");
            newPassword.InnerText = password;
            newUser.AppendChild(newPassword);


            XmlElement newName = doc.CreateElement("name");
            newName.InnerText = name;
            newUser.AppendChild(newName);

            XmlElement newLastName = doc.CreateElement("lastname");
            newLastName.InnerText = lastname;
            newUser.AppendChild(newLastName);



            doc.FirstChild.AppendChild(newUser);

            doc.Save("userdatabase.xml");

        }

        public XmlNode ReadItem(string cardkey, string XMLFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("userdatabase.xml");
            XmlNode currentUser = doc.SelectSingleNode("//user[@cardkey='" + cardkey + "']");
            return currentUser;
        }

        public void RemoveItem(string cardkey, string XMLFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("userdatabase.xml");
            doc.FirstChild.RemoveChild(doc.SelectSingleNode("//user[@cardkey='" + cardkey + "']"));
            doc.Save("userdatabase.xml");
        }

    }
}
