namespace Rinha.API.Core.Interfaces;

public interface ITranslator<in TIn, out TOut> where TIn : class where TOut : class
{
    TOut Translate(TIn input);
}