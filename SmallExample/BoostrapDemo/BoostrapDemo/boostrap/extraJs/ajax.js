function ajax(url, funSucc, funFaild){
    //1.创建ajax对象
    var ajaxObj = null;
    if(window.XMLHttpRequest){
        ajaxObj = new XMLHttpRequest();
    }else if(window.ActiveObject){
        ajaxObj = new ActiveXObject("Microsoft.XMLHTTP");
    }else{
        throw new Error('no ajax object available.')
}
      
    //2.连接服务器  
    ajaxObj.open('GET', URL, true);//open(方法, url, 是否异步)
      
    //3.发送请求  
    ajaxObj.send();
      
    //4.接收返回
    ajaxObj.onreadystatechange = function(){  //OnReadyStateChange事件
        if(ajaxObj.readyState == 4){  //4为完成
            if(ajaxObj.status == 200){    //200为成功
                funSucc(ajaxObj.responseText) 
            }else{
                if(funFaild){
                    funFaild();
                }
            }
        }
    };
}