﻿/* Pokemon: Infinte: A recreation of classic Pokemon games in C#
 * 
 * 
 * == Created ==
 * on: 5/23/2014
 * by: Eric Hopkins
 * 
 * == Modified ==
 * on: 8/10/2014
 * by: Eric Hopkins
 * 
 * This file contains all the data for the game engine class. As of
 * it's creation, this will only really involve reading in the games
 * data, however anything "base level" to the game may best be passed
 * through the game engine. The Engine Class file is static and cannot
 * be instantiated.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pokemon
{
    public static class Engine
    {

		#region xmlReaders
        public static void ReadPokemonXML(string fileLocation, ref LinkedList<Pokemon> list)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileLocation);

            foreach (XmlNode node in doc.DocumentElement)
            {
                

                if (node.NodeType != XmlNodeType.Comment)
                {
					int catchRate = int.Parse(node.ChildNodes.Item(1).InnerText);                    
					int dexNum = int.Parse(node.ChildNodes.Item(0).InnerText);
					string name = node.Attributes.GetNamedItem("name").Value;

                    // Levelgroup not yet used
                    bool genderless = node.ChildNodes.Item(3).InnerText != "0";

                    int[] types = new int[2];
                    int i = 0;
                    int typesCount = node.ChildNodes.Item(4).ChildNodes.Count;
					
                    foreach (XmlNode n in node.ChildNodes.Item(4))
                    {
                        types[i] = Convert.ToInt32(n.InnerText);
                        i++;
                    }

                    // Ensures that mono-type pokemon don't end up with a secondary normal type
                    if (typesCount != 2)
                    {
                        types[1] = -1;
                    }

                    int[] baseStats = new int[6];
                    int x = 0;
					
                    foreach (XmlNode n in node.ChildNodes.Item(5))
                    {
                        baseStats[x] = int.Parse(n.InnerText);
                        x++;
                    }

                    // TODO: attacks not yet implemented
					list.AddLast(new Pokemon(name, catchRate ,dexNum, genderless, baseStats, types));
                }
            }
        }


        public static void ReadTypeXML(string fileLocation)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileLocation);
            int i = 0;

            foreach (XmlNode node in doc.DocumentElement)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {   
                    string name = node.Attributes.GetNamedItem("name").Value;
                    double[] mods = new double[18];

                    int x = 0;
                    foreach(XmlNode n in node)
                    {
                        if(n.NodeType != XmlNodeType.Comment)
                        {
                            mods[x] = double.Parse(n.InnerText);
                            x++;
                        }
                    }

                    Types tempType = new Types(name, mods);
                    Model.Types[i] = new Types(tempType);
                    i++;
                }
            }
        }
		#endregion
    }
}
