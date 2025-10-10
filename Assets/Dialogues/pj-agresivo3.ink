-> introduccion

=== introduccion ===
¡Oye! ¡Déjenme entrar ahora mismo, no tengo todo el día!

+ [Calma, señor, tome distancia.]
    -> calma
+ [Si no se tranquiliza, no puedo permitirle el ingreso.]
    -> amenaza
-> END


=== calma ===
—¡No me importa! ¡Necesito entrar ya!

+ [Debe esperar su turno, no hay otra opción.]
    —¡Esto es un abuso! ¡Nunca me trataron así!
    -> END
+ [Tranquilo, respire hondo y no queremos problemas.]
    —...Está bien, pero apúrese, no tengo todo el día.
    -> END
+ [Si insiste así, tendré que llamar a seguridad.]
    —Bah, siempre tan estrictos, ¡pero igual voy a entrar!
    -> END
-> END


=== amenaza ===
—¡A mí nadie me dice lo que debo hacer!

+ [Última advertencia, retroceda.]
    —¡Ni hablar! ¡Voy a entrar igual!
    -> END
+ [Tranquilo, no queremos pelear.]
    —Tsk... siempre arruinando todo, ¿verdad?
    -> END
+ [Si intenta forzar la entrada, llamaré a seguridad.]
    —Ugh, qué molestia... pero igual voy a entrar.
    -> END
-> END


=== respuestaIngreso ===
—¡Por fin! Veo que alguien respeta a los enanos agresivos.
-> END

=== respuestaRechazo ===
—¡Esto es indignante! Nunca olvidaré esto...
-> END
