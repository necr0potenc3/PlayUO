namespace Client
{
    public class GShardMenu : GMenuItem
    {
        private ShardProfile m_Shard;

        public GShardMenu(ShardProfile shard) : base(shard.Name)
        {
            this.m_Shard = shard;
        }

        public override void OnClick()
        {
        }

        public ShardProfile Shard
        {
            get
            {
                return this.m_Shard;
            }
        }
    }
}