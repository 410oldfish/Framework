namespace GameLogic
{
    struct GridXY
    {
        public int X;
        public int Y;

        public GridXY(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    
    public static class Misc
    {
        public const int GOLD_ID = 1000001;
        public const int DIAMOND_ID = 1000002;
    }
}