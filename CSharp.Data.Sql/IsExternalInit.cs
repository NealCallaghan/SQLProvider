// There is a bug with some features of c#9 where by records can't have init parameters in constructors
// else error Failure CS0518  Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported
// for a fix see https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}
