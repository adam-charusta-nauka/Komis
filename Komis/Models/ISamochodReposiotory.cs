using System.Collections.Generic;

namespace Komis.Models
{
    public interface ISamochodReposiotory
    {
        IEnumerable<Samochod> PobierzWszystkieSamochody();
        Samochod PobierzSamochodPoId(int samochodId);
    }
}
