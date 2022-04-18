using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasExceptions
{
    ExceptionSegmentCollection Exceptions { get; set; }
}