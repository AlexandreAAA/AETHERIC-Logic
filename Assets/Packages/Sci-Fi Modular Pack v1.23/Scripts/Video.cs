using UnityEngine;
using System.Collections;

public class Video : MonoBehaviour
{

    public bool operate;
    public UnityEngine.Video.VideoClip videoClip;

    void Start()

    {

        var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
       var audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        videoPlayer.EnableAudioTrack(0, false); // no audio from videos
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

    }

    void Update() {
        if (operate)
        {
            var vp = GetComponent<UnityEngine.Video.VideoPlayer>();


            if (vp.isPlaying)
            { 
            vp.Pause();
            }
            else
            { 
            vp.Play();
            }
       
        }
    }
}