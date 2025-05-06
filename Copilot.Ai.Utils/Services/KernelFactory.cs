using System.Diagnostics.CodeAnalysis;
using Copilot.Ai.Utils.Interfaces;
using Copilot.Ai.Utils.Options;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace Copilot.Ai.Utils.Services;

public class KernelFactory : IKernelFactory
{
    private readonly OpenAiModelOptions _openAiModelOptions;

    public KernelFactory(
        IOptions<OpenAiModelOptions> openAiModelOptions)
    {
        _openAiModelOptions = openAiModelOptions.Value;
    }


    public Kernel Create(string gptModel)
    {
        var httpClient = new HttpClient();

        var kernel = Kernel.CreateBuilder()
            .AddOpenAITextEmbeddingGeneration(
                modelId: _openAiModelOptions.EmbeddingModelId,
                apiKey: _openAiModelOptions.ApiKey,
                httpClient: httpClient)
            .AddOpenAIChatCompletion(
                modelId: gptModel,
                apiKey: _openAiModelOptions.ApiKey,
                httpClient: httpClient)
            .Build();

        return kernel;
    }
}