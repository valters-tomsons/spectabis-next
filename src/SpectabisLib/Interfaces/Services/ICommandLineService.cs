using System.Threading.Tasks;

namespace SpectabisLib.Interfaces.Services
{
    public interface ICommandLineService
    {
        Task<bool> ExecuteArguments(string[] arguments);
    }
}