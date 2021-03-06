Sub Stock_Market()

'loop through worksheets
Dim ws As Worksheet
For Each ws In ThisWorkbook.Worksheets
    ws.Activate

    'testing
    'Application.DisplayAlerts = False

    'grab last row
    Dim LastRow As Long
    LastRow = Cells(Rows.Count, 1).End(xlUp).Row
    
    'allocate memory for variables
    Dim currentStock As String
    Dim openPrice As Double
    Dim closePrice As Double
    Dim priceDiff As Double
    Dim percentChange As Double
    Dim totalVolume As Variant

    'for tracking where to print results
    Dim printCount As Long

    'initialize
    printCount = 0
    openPrice = 0
    closePrice = 0
    priceDiff = 0
    percentChange = 0

    'loop through rows
    Dim i As Long
    For i = 2 To CLng(LastRow)
        ' conditional for new stock
        If Cells(i, 1) <> currentStock Then
            currentStock = Cells(i, 1)
            'special condition for first iteration of loop
            If i = 2 Then
                openPrice = Cells(i, 3)
            End If
            'if you dont have a string
            If VarType(Cells(i - 1, 6)) <> 8 Then
                closePrice = Cells(i - 1, 6)
                priceDiff = closePrice - openPrice
                openPrice = Cells(i, 3)
                'calculate as percentage if not dividing by zero
                If openPrice <> 0 Then
                    percentChange = (priceDiff / openPrice) * 100
                End If
            End If
            'iterate printCounter
            printCount = printCount + 1
            'print name, difference and percentage
            Cells(printCount + 1, 10) = currentStock
            Cells(printCount, 11) = priceDiff
            If priceDiff > 0 Then
                Cells(printCount, 11).Interior.Color = vbGreen
            ElseIf priceDiff < 0 Then
                Cells(printCount, 11).Interior.Color = vbRed
            End If
            
            Cells(printCount, 12) = percentChange & "%"
            'zero out total volume because its a new stock
            totalVolume = 0
        End If
        
        ' **HANDLE LAST ROW**
        If i = LastRow Then
            closePrice = Cells(i, 6)
            priceDiff = closePrice - openPrice
            If openPrice <> 0 Then
                    percentChange = (priceDiff / openPrice) * 100
            End If
            Cells(printCount + 1, 11) = priceDiff
            Cells(printCount + 1, 12) = percentChange
            If priceDiff > 0 Then
                Cells(printCount + 1, 11).Interior.Color = vbGreen
            ElseIf priceDiff < 0 Then
                Cells(printCount + 1, 11).Interior.Color = vbRed
            End If
        End If
        
        'add to total volume
        totalVolume = totalVolume + Cells(i, 7)
        'print value
        Cells(printCount + 1, 13) = totalVolume
        
    Next i
 
             
    'set headers
    Range("J1") = "<ticker>"
    Range("K1") = "<price difference>"
    Range("L1") = "<percentage change>"
    Range("M1") = "<total volume>"
    
    Range("O1") = ""
    Range("O2") = "<greatest increase>"
    Range("O3") = "<greatest decrease>"
    Range("O4") = "<greatest total volume>"
    
    Dim finalRow_totals As Long
    finalRow_totals = Range("L800000").End(xlUp).Row
    Dim rowString As String
    rowString = "L" + CStr(finalRow_totals)
    
    Dim dblMin As Double
    Dim dblMax As Double
    Dim sMin As String
    Dim sMax As String
    Dim varMax As Variant
    dblMin = Application.WorksheetFunction.Min(Range("L2", rowString))
    sMin = dblMin * 100
    dblMax = Application.WorksheetFunction.Max(Range("L2", rowString))
    sMax = dblMax * 100
    varMax = Application.WorksheetFunction.Max(Range("M2", rowString))
            
    Range("P1") = "<Ticker>"
    Range("Q1") = "<Value>"
    
    Range("Q2") = sMax
    Range("Q3") = sMin
    Range("Q4") = varMax

    'locate maxima and minima stock ticker names
    Dim position As Range
    Dim t_name As String
    
    
    On Error Resume Next
        Set position = Range("L:L").Find(CLng(dblMax))
        t_name = Cells(position.Row, position.Column - 2)
        Range("P2") = t_name
        Set position = Range("L:L").Find(CLng(dblMin))
        t_name = Cells(position.Row, position.Column - 2)
        Range("P3") = t_name
        
    
Next 'next sheet
End Sub



