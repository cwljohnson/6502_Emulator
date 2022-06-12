	F0 = $0001
	F1 = $0002
	FN = $0003

	LDX #00 ; initialize sequence with 0 and 1
	STX F0
	LDX #01
	STX F1

	LDY #00 ; initialize loop
	
loop:
	LDA F0
	ADC F1 ; F0 + F1

	LDX F1
	STX F0

	STA F1

	INY
	CPY #$0C ; Loop 12 times
	BNE loop

	STA FN ; save final number in FN
