<form id="form1" runat="server">

<h2>Quiz</h2>

<asp:Label ID="lblQuestion" runat="server"></asp:Label>

<br/>

<asp:RadioButtonList ID="rblAnswers" runat="server"></asp:RadioButtonList>

<br/>

<asp:Button ID="btnSubmitQuiz" runat="server" Text="Submit" OnClick="btnSubmitQuiz_Click"/>

<br/>

<asp:Label ID="lblResult" runat="server"></asp:Label>

</form>