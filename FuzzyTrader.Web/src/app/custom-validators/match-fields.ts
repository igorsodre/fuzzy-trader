import { AbstractControl, ValidationErrors } from '@angular/forms';

export const matchFields =
  (fieldOne: string, fieldTwo: string) =>
  (control: AbstractControl): ValidationErrors | null => {
    const password = control.get(fieldOne)?.value;
    const confirm = control.get(fieldTwo)?.value;

    if (password != confirm) {
      return { noMatch: true };
    }

    return null;
  };
