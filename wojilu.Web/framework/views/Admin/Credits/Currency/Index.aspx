

<div style="width:98%">


<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" >
	<tr>
		<td colspan="9" style="border:0px;" class="titleCell font14"><strong>_{keyCurrency}</strong></td>
	</tr>
	<tr class="tableHeader">
		<td>_{name}</td>
		<td>_{unit}</td>
		<td>_{exchangeRate}</td>
		<td>_{initValueWithReg}</td>
		<td>_{isShow}</td>
		<td>_{canDeal}</td>
		<td>_{canRate}</td>
		<td>_{editCurrency}</td>
		<td>_{admin}</td>
	</tr>
	<tr class="tableItems">
		<td>#{c.Name}</td>
		<td>#{c.Unit}</td>
		<td>-</td>
		<td>#{c.InitValue}</td>
		<td>#{c.IsShow}</td>
		<td>-</td>
		<td>#{c.CanRate}</td>
		<td><a href="#{c.EditUrl}" class="frmBox" title="_{edit}">_{edit}</a></td>
		<td>-</td>
	</tr>



	<tr>
		<td colspan="9">&nbsp;</td>
	</tr>


	<tr>
		<td colspan="9"><strong class="font14">_{otherCurrency}</strong><span class="left20"><a href="#{addUrl}" class="frmBox" title="_{addCurrency}">+_{addCurrency}</a></span></td>
	</tr>

	<tr class="tableHeader">
		<td>_{name}</td>
		<td>_{unit}</td>
		<td>_{exchangeRate}</td>
		<td>_{initValueWithReg}</td>
		<td>_{isShow}</td>
		<td>_{canDeal}</td>
		<td>_{canRate}</td>
		<td>_{editCurrency}</td>
		<td>_{admin}</td>
	</tr>
<!-- BEGIN list -->
	<tr class="tableItems">
		<td>#{s.Name}</td>
		<td>#{s.Unit}</td>
		<td>#{s.ExchangeRate}</td>
		<td>#{s.InitValue}</td>
		<td>#{s.IsShow}</td>
		<td>#{s.CanDeal}</td>
		<td>#{s.CanRate}</td>
		<td><a href="#{s.SetActionUrl}" class="frmBox" title="_{edit}">_{edit}</a></td>
		<td>#{s.DeleteUrl}</td>
	</tr>
<!-- END list -->

</table>

<div class="font12" style="margin:10px 0px;"></div>

</div>