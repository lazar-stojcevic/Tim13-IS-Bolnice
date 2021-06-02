using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IS_Bolnice
{
    public partial class App : Application
    {
        public ResourceDictionary SkinDictionary { get { return Resources.MergedDictionaries[0]; } }

        public void PromeniTemu(Uri uri)
        {
            SkinDictionary.MergedDictionaries.Clear();
            SkinDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });

        }
        public void OcistiTemu()
        {
            SkinDictionary.MergedDictionaries.Clear();
        }
    }
}
