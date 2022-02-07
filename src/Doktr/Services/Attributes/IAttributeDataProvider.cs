namespace Doktr.Services.Attributes;

public interface IAttributeDataProvider<out T>
{
    T Next();
}