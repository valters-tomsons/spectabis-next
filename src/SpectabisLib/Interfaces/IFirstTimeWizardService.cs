using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface IFirstTimeWizardService
    {
        Task WriteInitialConfigs();
        bool IsRequired();
        Task WriteFirstTimeWizardCompleted();
    }
}