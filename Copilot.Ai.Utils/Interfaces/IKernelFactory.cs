using Microsoft.SemanticKernel;

namespace Copilot.Ai.Utils.Interfaces;

public interface IKernelFactory
{
    Kernel Create(string gptModel);
}
