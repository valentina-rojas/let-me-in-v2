-> introduccion

=== introduccion ===
Hola, ¿cómo estás?

+ [¿De dónde viene con esa bandeja?]
    -> origenBandeja
+ [¿Tomó las precauciones necesarias en su cocina?]
    -> precaucionesCocina
+ [¿Por qué decidió venir en lugar de quedarse en su casa?]
    -> motivoVenir
-> END


=== origenBandeja ===
Ah, esto, traigo algo de comida casera para ofrecerle a todos, no sé si tienen alimentos en el búnker.

+ [Gracias por traer comida, pero debo revisarla primero.]
    —Claro, no hay problema, entiendo las reglas.
    -> END
+ [No podemos aceptar alimentos de afuera por seguridad.]
    —¡Oh, qué pena! Solo quería ayudar...
    -> END
-> END


=== precaucionesCocina ===
¡Por supuesto! Usé guantes, barbijo y desinfecté todo.

+ [¿Puede describir cómo lo preparó?]
    —Cociné en mi casa, todo fue en superficies limpias y con higiene.
    -> END
+ [Aun así necesito revisarlo antes de permitir el ingreso.]
    —Está bien, haga la revisión que considere necesaria.
    -> END
-> END


=== motivoVenir ===
Porque vivo sola y no sé cómo va a avanzar esto; prefiero estar en un lugar donde haya atención médica.

+ [Entiendo, aquí hay atención y seguridad, pero debo verificar su salud.]
    —Gracias, necesito estar cerca de ayuda por si pasa algo.
    -> END
+ [¿Tiene familia o alguien que la cuide?]
    —No, por eso vine. No quería quedarme sola.
    -> END
-> END


=== respuestaIngreso ===
¡Que Dios lo bendiga!
-> END

=== respuestaRechazo ===
Una vergüenza esta atención... ustedes se pierden de mis tartas
-> END
