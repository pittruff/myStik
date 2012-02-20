
using System;
using System.Collections.Generic;
using System.Text;


class HTMLCharacterConverter
{
    private Dictionary<char, string> criticals = new Dictionary<char, string>();

    public HTMLCharacterConverter()
    {

    }

    public String Purge(String text)
    {
        
        text= text.Replace("&nbsp;"," ");
        
        text = text.Replace("\r\n", " ");
        text = text.Replace("  ", " ");
        
        return text;
    }
}
