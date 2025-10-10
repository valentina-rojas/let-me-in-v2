-> introduccion

=== introduccion ===
—¡Déjenme entrar ahora mismo, no tengo todo el día!

+ [Señora, cálmese un momento, por favor.]
    -> calma

+ [Si no se tranquiliza, no puedo dejarla pasar.]
    -> amenaza
-> END


=== calma ===
—¡No me importa, necesito entrar ya!

+ [Lo siento, pero debe esperar su turno.]
    —¡Esto es un abuso! ¡Nunca me trataron así!
    -> END
+ [Si sigue así, tendré que llamar a seguridad.]
    —...Está bien, pero apúrese, no tengo todo el día.
    -> END
-> END


=== amenaza ===
—¡A mí nadie me dice lo que debo hacer!

+ [Última advertencia, retroceda.]
    —¡Ni hablar! ¡Voy a entrar igual!
    -> END
+ [Tranquila, no queremos problemas.]
    —Tsk... siempre arruinando todo, ¿verdad?
    -> END
-> END


=== respuestaIngreso ===
—¡Por fin! Veo que alguien respeta a los ancianos.
-> END

=== respuestaRechazo ===
—¡Esto es indignante! Nunca olvidaré esto...
-> END
