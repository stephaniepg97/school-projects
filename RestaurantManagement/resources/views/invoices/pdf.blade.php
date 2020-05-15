<!DOCTYPE html>
<html lang="en">

    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>Invoice no.: {{$invoice->id}}</title>
    </head>
    <body>

    	<h1>Invoice no.: {{$invoice->id}}</h1>

    	<div class="form-group">
    		<label><b>State:</b> {{$invoice->state}}</label>
	    </div>
	    <div class="form-group">
	        <label><b>NIF:</b> {{$invoice->nif}}</label>
	    </div>
	    <div class="form-group">
	        <label><b>Customer's Name:</b> {{$invoice->name}}</label>
	    </div>
	    @if ($invoice->table())
	    <div class="form-group">
	        <label><b>Table:</b> {{$invoice->table()}}</label>
	    </div>
	    @else
	    <div class="form-group">
	        <label><b>Table:</b> <em>TABLE DELETED</em></label>
	    </div>
	    @endif
	    @if ($invoice->waiter())
	    <div class="form-group">
	        <label><b>Responsible Waiter:</b> {{$invoice->waiter()}}</label>
	    </div>
	    @else
	    <div class="form-group">
	        <label><b>Responsible Waiter:</b> <em>USER DELETED</em></label>
	    </div>
	    @endif
	    <div class="form-group">
	        <label><b>Date:</b> {{$invoice->date}}</label>
	    </div>
	    @if(count($invoice->receipt()))
	    <div class="form-group">
	        <table>
	        	<thead>
		        <tr>
		            <th>Items</th>
		        </tr>
			    </thead>
		        <thead>
			        <tr>
			            <th>Name</th>
			            <th>Quantity</th>
			        	<th>Price Unit</th>
			        	<th>Subtotal Price</th>
			        </tr>
			    </thead>
			    <tbody>
			    	@foreach($invoice->receipt() as $line)
			    	<tr>
			    		@if($invoice->item($line->item_id))
			    		<td>{{$invoice->item($line->item_id)->name}}</td>
			    		<td>{{$line->quantity}}</td>
			    		<td>{{$line->unit_price}}</td>
			    		<td>{{$line->sub_total_price}}</td>
			    		@else
			    		<td><em>ITEM DELETED</em></td>
			    		@endif
			    	</tr>
			    	@endforeach
			    </tbody>
	        </table>
	    </div>
	    @endif
	    <div class="form-group">
	        <label>Total Price: {{$invoice->total_price}}</label>
	    </div>
    </body>
</html>


