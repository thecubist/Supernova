using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace SuperNova
{
    class Sound
    {
        
        SoundEffect soundEffect;

        public void Initialize(SoundEffect laserSound)
        {
            soundEffect = laserSound;
            SoundEffect.MasterVolume = 0.3f;
            
        }

        public void playSound()
        {
            
            soundEffect.CreateInstance();
            soundEffect.Play();
            
        }
    }
}
