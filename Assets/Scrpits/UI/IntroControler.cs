using UnityEngine;
using UnityEngine.Playables;

public class IntroControler : MonoBehaviour
{
  public PlayableDirector director;
  
  public ObjectEventSO loadMenuEvent;

  private void Awake()
  {
    //director = GetComponent<PlayableDirector>();
    //播放结束后事件
    director.stopped += OnPlayableDirectorStopped;
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space)&&director.state == PlayState.Playing)
      director.Stop();
  }
  private void OnPlayableDirectorStopped(PlayableDirector director)
  {
    loadMenuEvent.RaiseEvent(null,this);
  }
}
