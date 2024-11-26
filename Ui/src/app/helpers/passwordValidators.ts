import {AbstractControl, ValidationErrors, ValidatorFn} from "@angular/forms";

export class PasswordValidators {
  // Validator function to check if password and confirmPassword match
  static passwordMatch(password: string, confirmPassword: string): ValidatorFn {
    return (formGroup: AbstractControl): { [key: string]: any } | null => {
      const passwordControl: AbstractControl<any, any> | null = formGroup.get(password);
      const confirmPasswordControl: AbstractControl<any, any> | null = formGroup.get(confirmPassword);

      // Check if controls are available
      if (!passwordControl || !confirmPasswordControl) {
        return null;
      }

      // Check if there is an existing 'passwordMismatch' error
      if (
        confirmPasswordControl.errors &&
        !confirmPasswordControl.errors["passwordMismatch"]
      ) {
        return null;
      }

      // Check if password and confirmPassword values match
      if (passwordControl.value !== confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({passwordMismatch: true});
        return {passwordMismatch: true};
      } else {
        confirmPasswordControl.setErrors(null);
        return null;
      }
    };
  }

  // Validator function to check if the control value matches a given regex pattern
  static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [p: string]: any } | null => {
      // If the control is empty, return no error
      if (!control.value) {
        return null;
      }

      // Test the value of the control against the supplied regex
      const valid = regex.test(control.value);

      // If true, return no error; otherwise, return the specified error
      return valid ? null : error;
    };
  }
}
