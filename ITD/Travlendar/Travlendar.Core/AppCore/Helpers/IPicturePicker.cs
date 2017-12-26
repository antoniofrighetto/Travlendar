using System.IO;
using System.Threading.Tasks;

namespace Travlendar.Core.AppCore.Helpers
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync ();
    }
}
