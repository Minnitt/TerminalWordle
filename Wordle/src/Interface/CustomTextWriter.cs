using System.Text;

namespace Wordle.Interface;

public class CustomTextWriter : TextWriter
{
    private TextWriter original;
    private TextWriter mirror;
    private int height;
    private int width;
    public override Encoding Encoding => original.Encoding;
    
}