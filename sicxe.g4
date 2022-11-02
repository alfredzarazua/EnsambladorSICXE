grammar sicxe;								//nombre de la gramatica

/*
*opciones de compilacion de la gramatica
*/
options {							
    language=CSharp2;								//lenguaje objetivo de la gramatica
}

/*
*	Reglas del Parser
*/

programa					
	: inicio
	;

inicio
	: simbolo 'START' CONS NEWLINE*
	;

expresion returns[string[] value = new string[2]]
    : simbolo instruct NEWLINE{$value=$instruct.value;}
    ;

expresion2
    : 'END' ope? NEWLINE*
    ;

instruct returns[string[] value = new string[3]]
    : FORMATO1 {$value[1] = "1"; $value[2] = "1";}|forma2 {$value[0]=$forma2.value[0];$value[1]=$forma2.value[1];$value[2]="1";}|
		simple {$value[0]=$simple.value[0];$value[1]=$simple.value[1];$value[2]="1";}|indirecto {$value[0]=$indirecto.value[0];$value[1]=$indirecto.value[1];$value[2]="1";}|
		inmediato {$value[0]=$inmediato.value[0];$value[1]=$inmediato.value[1];$value[2]="1";}|directivo {$value[1]=$directivo.value[1];}|
		dirrsub {$value[1]=$dirrsub.value; $value[2]="1";}
    ;
 
 simple returns[string[] value = new string[2]]
    : index {$value[0]="simple"; $value[1] = $index.value;} |noindex {$value[0]="simple"; $value[1] = $noindex.value;}
    ;

index returns[string value]
    : normal ope ',' EQUIS {$value=$normal.value;} |exten ope ',' EQUIS {$value=$exten.value;}
    ;

ope
    : CONS|ETIQ
    ;

normal returns[string value]
    : FORMATO34 {$value = "3";}
    ;

exten returns[string value]
    : '+' FORMATO34 {$value = "4";}
    ;

noindex returns[string value]
    : normal ope {$value=$normal.value;}| exten ope {$value=$exten.value;}
    ;

indirecto returns[string[] value = new string[2]]
    : normal '@' ope {$value[0]="indirecto"; $value[1] = $normal.value;}|exten '@' ope {$value[0]="indirecto"; $value[1] = $exten.value;}
    ;

inmediato returns[string[] value = new string[2]] 
    : normal '#' ope {$value[0]="inmediato"; $value[1] = $normal.value;}| exten '#' ope {$value[0]="inmediato"; $value[1] = $exten.value;}
    ;

forma2 returns[string[] value=new string[2]]
    : insunreg {$value[1]="2";}| insdosreg {$value[1]="2";}| insnumreg {$value[1]="2";}|insnumnoreg {$value[1]="2";}
    ;

insunreg
    : FORMATO2UNREG registros
    ;

insdosreg
    : FORMATO2DOSREG registros ',' registros
    ;

insnumreg
	: FORMATO2REGNUM registros ',' CONS
	;

insnumnoreg
	: FORMATO2NUMNOREG CONS
	;

registros
	: REG | EQUIS
	;

simbolo
    : ETIQ |
    ;

directivo returns[string[] value = new string[2]]
	: normaldirec {$value[1]=$normaldirec.value;}|specdirec {$value[1]=$specdirec.value;}
	;

normaldirec returns[string value]
	: direcnum {$value=$direcnum.value;}|direcsimb|direqu
	;

direcnum returns[string value]
	: WORD CONS{$value="3";}|RESB CONS{$value = $CONS.text;}|RESW CONS{$value = (Global.HexHtoInt($CONS.text)*3).ToString();}
	;

direqu
	: EQDIR exprcalc
	;


direcsimb
	: DIRBAS ETIQ
	;

specdirec returns[string value]
	: BYTDIR tipores {$value=$tipores.value;}
	;


tipores returns[string value]
	: 'C' '\'' ETIQ '\'' {$value = $ETIQ.text.Length.ToString();}| 'X' '\'' CONS '\''  {$value = (System.Math.Floor((double)($CONS.text.Length/2))*2).ToString();}
	;

dirrsub returns[string value]
	: RSUB {$value="3";}| '+' RSUB {$value="4";}
	;

exprcalc returns[int[] value = new int[2]]						//El valor calculado por la expresion sera regresado como un entero.
	:	
	a = multiplicacion{$value = $a.value;} (		//Se asina el valor que se retornara en la regla.
	MAS b = multiplicacion {$value[0] =$value[0]+ $b.value[0]; $value[1] =$value[1] + $b.value[1];}				//El valor se suma con el actual en la expresion.
	|
	MENOS b = multiplicacion{$value[0] =$value[0]- $b.value[0]; $value[1] =$value[1] - $b.value[1];})*	//El valor se resta con el actual y se imprime el valor.
	;

multiplicacion returns[int[] value = new int[2]]					//La regla retorna un entero.
	:	
	a = numero{$value = $a.value;}  (				//Se asigna el valor que se regresara.
	POR b = numero{$value[0] =$value[0]* $b.value[0]; $value[1] =$value[1] + $b.value[1];}		//Se calcula la multiplicacion 
	|
	ENTRE b = numero{$value[0] =$value[0]/ $b.value[0]; $value[1] =$value[1] + $b.value[1];})*	//Se calcula la division.
	;

numero returns[int[] value = new int[2]]							//La regla retonara un entero.
	:	
	tbsimbolo	{$value = $tbsimbolo.value;}			//se convierte a entero la cadena de entrada de la consola.			
	|
	MENOS tbsimbolo {$value[0] = -1*$tbsimbolo.value[0]; $value[1] = -1*$tbsimbolo.value[1];}
	|	
	PARENI exprcalc PAREND		{$value = $exprcalc.value;}		//se asigna el valor de la expresion dentro del parentesis.
	|
	MENOS PARENI exprcalc PAREND {$value[0] = -1*$exprcalc.value[0]; $value[1] = -1*$exprcalc.value[1];}
	;

tbsimbolo returns[int[] value = new int[2]]
	:
	ETIQ /*{$value[0]=Program.getDir($ETIQ.text); $value[1]=Program.esRoA($ETIQ.text);}*/
	;

/*
*	Reglas del Lexer.
*/

EQUIS
	: 'X'
	;

FORMATO1
    : ('FIX'|'FLOAT'|'HIO'|'NORM'|'SIO'|'TIO')
    ;

FORMATO34
    : ('ADD'|'COMP'|'J'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDB'|'LDCH'|'LDS'|'LDT'|'LDX'|'LPS'|'MUL'|'MULF'|'RD'|'STA'|'STCH'
	  |'SUB'|'TIX'|'WD')
    ;

FORMATO2UNREG
    : ('CLEAR'|'TIXR')
    ;

FORMATO2DOSREG
    : ('ADDR'|'COMPR'|'MULR'|'RMO'|'SUBR')
    ;

FORMATO2REGNUM
	: ('SHIFTL'|'SHIFTR')
	;

FORMATO2NUMNOREG
	: 'SVC'
	;

REG
    : ('A'|'L'|'B'|'S'|'T'|'F')
    ;

WORD
	: 'WORD'
	;

RESB
	: 'RESB'
	;

RESW
	: 'RESW'
	;

DIRBAS
	: 'BASE'
	;

BYTDIR
	: 'BYTE'
	;

RSUB
	: 'RSUB'
	;

EQDIR
	: 'EQU'
	;


PARENI
	:	'('		//token de parentesis derecho
	;
PAREND
	:	')'		//token de parentesis izquierdo.
	;
MAS 
	: '+'		//token de signo mas
	;
MENOS 
	: '-'		//token de signo menos
	;
POR
	: '*'		//token de signo por
	;

ENTRE
	: '/'		//token de signo entre
	;

ETIQ
    : ('a'..'z'|'A'..'Z') ('a'..'z'|'A'..'Z'|'0'..'9')*
    ;

CONS
    : (('a'..'f'|'A'..'F'|'0'..'9')+)('H'|)  
    ;

NEWLINE: ('\r'? '\n' | '\r')+ ;

WS
	: (' '|'\t')+ {Skip();}	//tokens que identifican las secuencas de escape.
	;
