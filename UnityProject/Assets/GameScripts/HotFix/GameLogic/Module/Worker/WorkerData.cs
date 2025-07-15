namespace GameLogic
{
    //每个雇员的属性
    public class WorkerData
    {
        private int id;

        private int name;
        private int lv;
        private int exp;
        
        //经营 烹饪 研发 种植 畜牧 捕鱼 
        private int mgmt = 0;
        private int cook = 0;
        private int research = 0;
        private int farm = 0;
        private int ranch = 0;
        private int fishing = 0;
    }
}