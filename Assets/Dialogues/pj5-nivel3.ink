-> introduccion

=== introduccion ===
¡No te preocupes! Estoy limpio por dentro y por fuera… me comí una barra entera antes de venir ¡No hay virus que sobreviva!

+ [Nombre y motivo de ingreso al búnker.]
    -> nombreMotivo

+ [¿Está al tanto de que comer jabón no lo hace inmune al virus?]
    -> comerJabon

+ [¿Y qué piensa hacer si se termina el jabón acá adentro?]
    -> sinJabon
-> END


=== nombreMotivo ===
Me llamo Elías Lavanda. Vengo por refugio... y si tienen jabón líquido mejor. ¡Es más fácil de tragar!

+ [Gracias por la información, pero debo verificar su estado de salud.]
    —No hay problema, estoy sano y limpio como una patena.
    -> END
+ [Solo podemos dejar ingresar a quienes sigan las normas del búnker.]
    —¡Normas aburridas! Pero lo entiendo, igual quiero entrar.
    -> END
-> END


=== comerJabon ===
¡Ja! Eso es lo que ellos quieren que creas, yo llevo seis meses comiendo jabón y todavía no estornudé ni una sola vez.

+ [Eso no garantiza nada, podría estar enfermo.]
    —No estoy enfermo, lo juro. Mis ojos rojos son por alergia al detergente, no virus.
    -> END
+ [Debo negar el ingreso hasta que se haga un chequeo.]
    —Ugh, siempre arruinando la diversión. Bueno, igual quiero entrar.
    -> END
-> END


=== sinJabon ===
Improvisar, ya estoy entrenando el paladar con detergente de limón. Es picante, pero uno se acostumbra…

+ [No podemos permitir que consuma productos peligrosos dentro del búnker.]
    —Tranquilo, solo planeo sobrevivir con estilo.
    -> END
+ [Entonces no puedo dejarlo entrar sin supervisión.]
    —Bah, siempre tan estrictos. Está bien, pero igual voy a entrar.
    -> END
-> END


=== respuestaIngreso ===
Gracias, espero que acá haya buenos productos de limpieza…
-> END

=== respuestaRechazo ===
Bueno, de todos modos estoy protegido
-> END
