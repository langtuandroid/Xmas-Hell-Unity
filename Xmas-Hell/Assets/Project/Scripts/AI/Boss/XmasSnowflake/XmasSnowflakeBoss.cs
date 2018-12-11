using System.Collections.Generic;

public class XmasSnowflakeBoss : AbstractBoss
{
    private List<XmasSnowflakeBranch> _branches = new List<XmasSnowflakeBranch>(8);

    public List<XmasSnowflakeBranch> Branches => _branches;

    public override void Pause()
    {
        base.Pause();

        foreach (var branch in _branches)
            branch.Pause();
    }

    public override void Resume()
    {
        base.Resume();

        foreach (var branch in _branches)
            branch.Resume();
    }

    public void AddBranch(XmasSnowflakeBranch branch)
    {
        _branches.Add(branch);
    }

    public void DestroyBranches()
    {
        for (int i = 0; i < _branches.Count; i++)
        {
            if (_branches[i] != null)
            {
                Destroy(_branches[i].gameObject);
            }
        }
    }
}
