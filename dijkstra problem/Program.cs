// using System.Collections;

string input = "Plzeň,Brno,296\nOlomouc,Brno,77\nČeské Budějovice,Brno,217\nHradec Králové,Brno,167\nÚstí nad Labem,Brno,298\nPardubice,Brno,146\nZlín,Brno,96\nOstrava,Brno,170\nKarlovy Vary,Brno,337\nJihlava,Brno,93\nPraha,Brno,209\nPaskov,Brno,176\nKladno,Brno,240\nLiberec,Plzeň,198\nČeské Budějovice,Plzeň,137\nÚstí nad Labem,Plzeň,181\nZlín,Plzeň,389\nKarlovy Vary,Plzeň,82\nJihlava,Plzeň,218\nPraha,Plzeň,91\nKladno,Plzeň,99\nLiberec,Olomouc,237\nČeské Budějovice,Olomouc,290\nHradec Králové,Olomouc,140\nPardubice,Olomouc,135\nZlín,Olomouc,62\nOstrava,Olomouc,94\nJihlava,Olomouc,167\nPaskov,Olomouc,100\nČeské Budějovice,Liberec,250\nHradec Králové,Liberec,97\nÚstí nad Labem,Liberec,96\nPraha,Liberec,109\nKladno,Liberec,135\nÚstí nad Labem,České Budějovice,238\nPardubice,České Budějovice,193\nZlín,České Budějovice,308\nOstrava,České Budějovice,383\nKarlovy Vary,České Budějovice,218\nJihlava,České Budějovice,138\nPraha,České Budějovice,150\nPaskov,České Budějovice,388\nKladno,České Budějovice,181\nÚstí nad Labem,Hradec Králové,190\nPardubice,Hradec Králové,24\nKarlovy Vary,Hradec Králové,241\nJihlava,Hradec Králové,113\nPraha,Hradec Králové,116\nKladno,Hradec Králové,143\nPardubice,Ústí nad Labem,196\nZlín,Ústí nad Labem,389\nKarlovy Vary,Ústí nad Labem,121\nJihlava,Ústí nad Labem,218\nPraha,Ústí nad Labem,92\nKladno,Ústí nad Labem,86\nKarlovy Vary,Pardubice,246\nJihlava,Pardubice,90\nPraha,Pardubice,121\nKladno,Pardubice,148\nOstrava,Zlín,125\nKarlovy Vary,Zlín,429\nJihlava,Zlín,186\nPraha,Zlín,301\nPaskov,Zlín,131\nKladno,Zlín,332\nJihlava,Ostrava,259\nPaskov,Ostrava,13\nPraha,Karlovy Vary,128\nKladno,Karlovy Vary,105\nPraha,Jihlava,130\nPaskov,Jihlava,266\nKladno,Jihlava,161\nKladno,Praha,31";
string[] inputArr = input.Split('\n');
List<City> cityList = new List<City>();
foreach (string line in inputArr)
{
	var lineParts = line.Split(",");
	if (!cityList.Any(x => x.CityName.Equals(lineParts[0])))
	{
		cityList.Add(new City(lineParts[0]));
	}
	if (!cityList.Any(x => x.CityName.Equals(lineParts[1])))
	{
		cityList.Add(new City(lineParts[1]));
	}
	City city1 = cityList.Where(x => x.CityName == lineParts[0]).First();
	City city2 = cityList.Where(x => x.CityName == lineParts[1]).First();
	city1.Roads.Add(city2, Convert.ToInt32(lineParts[2]));
	city2.Roads.Add(city1, Convert.ToInt32(lineParts[2]));

}
int[,] paths = new int[cityList.Count(), 4];
//paths[x,0] index
//paths[x,1] visited
//paths[x,2] Cost (length of the connection)
// paths[x,3] Path (how did i get to it with the cost)

string startCity = "Plzeň";
string endCity = "Olomouc";
for (int i = 0; i < cityList.Count; i++)
{
	paths[i, 0] = i;
	paths[i, 1] = 0;
	if (cityList[i].CityName == startCity)
	{
		paths[i, 2] = 0;
	}
	else
	{
		paths[i, 2] = int.MaxValue;
	}

	paths[i, 3] = -1;
}

for (int i = 0; i < cityList.Count; i++)
{
	int closest = 0;
	for (int j = 0; j < cityList.Count; j++)
	{
		if (paths[j, 0] == 0 && paths[j, 2] < closest)
		{
			closest = paths[j, 0];
			paths[j, 0] = 1;
		}
	}
	var closestCity = cityList[closest];
	foreach (KeyValuePair<City, int> road in closestCity.Roads)
	{
		var indexCity1 = cityList.IndexOf(closestCity);
		var indexCity2 = cityList.IndexOf(cityList.Where(x => x.CityName.Equals(road.Key.CityName)).First());
		if (paths[indexCity2, 1] != 1 && paths[indexCity2, 2] > road.Value)
		{
			paths[indexCity2, 2] = road.Value;
			paths[indexCity2, 3] = indexCity1;
		}
	}
}
var indexStartCity = cityList.IndexOf(cityList.Where(x => x.CityName.Equals(startCity)).First());
var indexEndCity = cityList.IndexOf(cityList.Where(x => x.CityName.Equals(endCity)).First());
int vzdalenost = 0;
string path = "";
int laststep = indexEndCity;
bool unfound = true;
while (unfound)
{
	vzdalenost += paths[laststep, 2];
	path += cityList[laststep].CityName;
	laststep = paths[indexEndCity, 3];
	if (laststep == indexStartCity || laststep == -1)
	{
		unfound = false;
	}
	Console.WriteLine(path);
	Console.WriteLine(vzdalenost);
}
Console.WriteLine(path);
Console.WriteLine(vzdalenost);
public class City
{
	public string CityName { get; set; }
	public Dictionary<City, int> Roads { get; set; }
	public City(string cityName)
	{
		CityName = cityName;
		Roads = new Dictionary<City, int>();
	}

}