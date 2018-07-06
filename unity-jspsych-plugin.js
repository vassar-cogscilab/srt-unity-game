jsPsych.plugins['unity-loader'] = (function() {
  var plugin = {};
  plugin.info = {
    name: 'unity-loader',
    parameters: {
      gameJSON: {
        type: jsPsych.plugins.parameterType.STRING,
        default: undefined
      }
    }
  }

  plugin.trial = function(display_element,trial){
    var html =   '<div class="webgl-content">'+
      '<div id="gameContainer" style="width: 960px; height: 600px"></div>'+
      '</div>'
    display_element.innerHTML = html;


    var gameInstance = UnityLoader.instantiate("gameContainer", trial.gameJSON, {onProgress: UnityProgress});


    window.unityListerner = function(msg){
      alert(msg);
    }

    window.unityData = [];

    window.unityGetData = function(json_data){
      window.unityData.push(JSON.parse(json_data));
    }

    window.unityEndGame = function(){
      display_element.innerHTML = '';

      jsPsych.finishTrial({
        unity_data: JSON.stringify(window.unityData)
      });
    }
  }
  return plugin;
})();
