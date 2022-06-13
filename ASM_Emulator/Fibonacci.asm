	F0L = $0000
	F0H = $0001
	F1L = $0002
	F1H = $0003
	FNL = $0010
	FNH = $0011

	LDX #$00 ; initialize sequence with 0 and 1
	STX F0H
	STX F1H

	LDX #$00
	STX F0L

	LDX #$01
	STX F1L

	LDY #$00 ; initialize loop
	
loop:
	CLC

	LDA F0L ; Add LSB
	ADC F1L ; F0L + F1L

	LDX F1L ; Shift LSB around
	STX F0L

	STA F1L

	LDA F0H ; Add MSB
	ADC F1H ; F0H + F1H

	LDX F1H : Shift MSB around
	STX F0H

	STA F1H

	INY
	CPY #$13 ; Loop 12 times
	BNE loop

	LDX F1L
	STX FNL

	LDX F1H
	STX FNH
