using UnityEngine;

public abstract class AbstractAnimal : AbstractLivingBeing
{
    private AnimalSoundtrack _AnimalSoundtrack;
    public AnimalSoundtrack AnimalSoundtrack
    {
        get
        {
            if(!_AnimalSoundtrack)
                _AnimalSoundtrack = GameObject.RequireComponentAdd<AnimalSoundtrack>();
           
            return _AnimalSoundtrack;
        }
    }

    public int _isDead;

    public bool isDead()
    {
        if(_isDead == 0)
            return false;
        else
            return true;
    }

    new public void Reset()
    {
        base.Reset();
        SetupData();
    }

    new public void Awake()
    {
        base.Awake();
        SetupData();
    }

    private void SetupData()
    {
        _isDead = 0;
    }
}