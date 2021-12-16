using Day15;


var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var map = inputString.Split(new[] { '\n', '\r'}).Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

int startX = 0;
int startY = 0;





