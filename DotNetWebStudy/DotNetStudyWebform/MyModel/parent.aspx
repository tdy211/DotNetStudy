<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="parent.aspx.cs" Inherits="DotNetStudyWebform.MyModel.parent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function OpenWin() {
            window.open("child.aspx", "new")//打开子窗口函数
        }

        function shuaxin(obj)//这个函数从子窗口调用，obj是接收到的对象
        {
            document.getElementById("getObjFromChild").value=obj.name+","+obj.age;//获取对象属性。
        }

        function shuaxinb() {
            window.location.href = window.location.href;
        }
    </script>
</head>
<body>
    <h1>父窗口</h1>
    <form id="form1" runat="server">
        <div>
            <span>从子窗口获取值<input type="text" id="getDataFromChild" style="width: 323px" /></span><br>
            <span>从子窗口获对象<input type="text" id="getObjFromChild" style="width: 323px" /></span><br>
            <input type="button" id="d" value="打开子窗口" onclick="javascript: OpenWin();" /><br>
            <input type="button" id="Button1" value="刷新" onclick="javascript: shuaxinb();" /></br>
            

        </div>
    </form>
</body>
</html>
