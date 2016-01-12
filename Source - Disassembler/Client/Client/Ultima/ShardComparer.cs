namespace Client
{
    using System;
    using System.Collections;

    public class ShardComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            ShardProfile shard = null;
            ShardProfile profile2 = null;
            if (x is GShardMenu)
            {
                shard = ((GShardMenu)x).Shard;
            }
            else
            {
                shard = x as ShardProfile;
            }
            if (y is GShardMenu)
            {
                profile2 = ((GShardMenu)y).Shard;
            }
            else
            {
                profile2 = y as ShardProfile;
            }
            if (shard == null)
            {
                return 1;
            }
            if (profile2 == null)
            {
                return -1;
            }
            TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
            DateTime now = DateTime.Now;
            int hours = currentTimeZone.GetUtcOffset(now).Hours;
            int num2 = -shard.TimeZone;
            int num3 = -profile2.TimeZone;
            int num4 = Math.Abs((int)(hours - num2));
            int num5 = Math.Abs((int)(hours - num3));
            int num6 = num4 - num5;
            if (num6 == 0)
            {
                num6 = shard.Name.CompareTo(profile2.Name);
                if (num6 == 0)
                {
                    num6 = shard.Index - profile2.Index;
                }
            }
            return num6;
        }
    }
}