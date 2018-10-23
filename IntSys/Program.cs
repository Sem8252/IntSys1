using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntSys
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Unit> units = GetUnitsFromFile();
            string start = Console.ReadLine();
            string finish = Console.ReadLine();
            List<Path> path = new List<Path>();
            switch(Console.Read())
            {
                case '1':
                    path = bypassWide(units, start, finish);
                    break;
                case '2':
                    path = bypassDeep(units, start, finish);
                    break;
            }
            var way = GetWay(path,start,finish);
            foreach (var a in way) Console.Write(a);
        }

        static List<Path> bypassWide(List<Unit> units, string start, string finish)
        {
            List<Path> path = new List<Path>();
            List<Unit> unit = units;
            List<Unit> open = new List<Unit>();
            open.Add(unit.Find(x => x.name.Contains(start)));
            List<Unit> closed = new List<Unit>();
            int count = 0;
            while(open.Count>0)
            {
                if (open[0].name == finish)
                    break;
                if (!open[0].childNames.Contains("0"))
                {
                    count++;
                    foreach (string current in open[0].childNames)
                    {
                        try
                        {
                            var next = unit.Find(x => x.name.Contains(current));
                            if (!closed.Contains(next))
                                open.Add(next);
                            path.Add(new Path(open[0].name, next.name));
                        }
                        catch { }
                    }
                }
                closed.Add(open[0]);
                open.RemoveAt(0);
            }
            Console.WriteLine(count);
            return path;
        }

        static List<Path> bypassDeep(List<Unit> units, string start, string finish)
        {
            List<Path> path = new List<Path>();
            List<Unit> unit = units;
            List<Unit> open = new List<Unit>();
            open.Add(unit.Find(x => x.name.Contains(start)));
            List<Unit> closed = new List<Unit>();
            int count = 0;
            while (open.Count > 0)
            {
                if (open[0].name == finish)
                    break;
                var buff = open[0];
                if (!open[0].childNames.Contains("0"))
                {
                    count++;
                    foreach (string current in open[0].childNames)
                    {
                        try
                        {
                            var next = unit.Find(x => x.name.Contains(current));
                            if (!closed.Contains(next))
                                open.Insert(0, next);
                            path.Add(new Path(buff.name, next.name));
                        }
                        catch { }
                    }
                }
                closed.Add(buff);
                open.Remove(buff);
            }
            Console.WriteLine(count);
            return path;
        }

        static List<string> GetWay(List<Path> path, string start, string finish)
        {
            List<string> way = new List<string>();
            var currentUnit = path.Find(x => x.Out.Contains(finish));
            way.Add(finish);
            way.Add(currentUnit.In);
            while(currentUnit.In!=start)
            {
                currentUnit = path.Find(x => x.Out.Contains(currentUnit.In));
                way.Add(currentUnit.In);
            }
            way.Reverse();
            return way;
        }

        static List<Unit> GetUnitsFromFile()
        {
            List<Unit> units = new List<Unit>();
            string path = "units.csv";
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var splited = line.ToString().Split(':');
                var child = splited[1].Split(',');
                Unit current = new Unit(splited[0],child.ToList<string>());
                units.Add(current);
            }
            return units;
        }
    }

    class Unit
    {
        public string name;
        public List<string> childNames;
        public Unit(string name, List<string> childs)
        {
            this.name = name;
            this.childNames = childs;
        }
    }
    class Path
    {
        public string In;
        public string Out;
        public Path(string from, string to)
        {
            In = from;
            Out = to;
        }
    }
}
