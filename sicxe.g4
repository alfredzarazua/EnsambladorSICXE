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
	: direcnum {$value=$direcnum.value;}|direcsimb
	;

direcnum returns[string value]
	: WORD CONS{$value="3";}|RESB CONS{$value = $CONS.text;}|RESW CONS{$value = (Global.HexHtoInt($CONS.text)*3).ToString();}
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
