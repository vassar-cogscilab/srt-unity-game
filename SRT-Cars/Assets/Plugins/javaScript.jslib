mergeInto(LibraryManager.library, {

  Upload: function (str) {
    window.unityGetData(Pointer_stringify(str));
  },

  EndGame: function(){
    window.unityEndGame();
  },

});
