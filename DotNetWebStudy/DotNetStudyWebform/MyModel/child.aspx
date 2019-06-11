<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="child.aspx.cs" Inherits="DotNetStudyWebform.MyModel.child" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script >
        function closew()
        {
           var parent= window.opener;
 
          var person={};
          person.name=document.getElementById("name").value;
          person.age = document.getElementById("age").value;
          parent.shuaxin(person);//把person对象传递过去
          var parentControl = parent.document.getElementById("getDataFromChild");
          parentControl.value = document.getElementById("otherContent").value;;
          window.close();
        }
    </script>
</head>
<body>
    <h1>子窗口</h1>
    <form id="form1" runat="server">
      <div>
         <span> 对象的姓名：<input type="text" id="name" /></span><br>
         <span> 对象的年龄：<input type="text" id="age" /></span><br>
          <span> 需要传递的单值：<input type="text" id="otherContent" /></span><br>
        <input type="button" id="d" value="提交给父窗口并关闭" onclick="javascript: closew();" />
    </div>
    </form>
</body>
</html>
