import * as React from "react";
import * as NewGroupBuyingFormStyles from "./NewGroupBuyingFormStyles";
import { Stack, Dropdown, Label } from "office-ui-fabric-react";
import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  IDatePickerStrings,
  IDropdownOption,
  PrimaryButton,
  Separator,
  Text,
  TextField,
  Image,
} from "@fluentui/react";
import { CategoriesMap } from "../Model/Categories";

export const NewGroupBuyingForm: React.FunctionComponent = () => {
  const [formInputs, setFormInputs] = React.useReducer<
    (
      prevState: NewGroupBuyDetails,
      state: NewGroupBuyDetails
    ) => NewGroupBuyDetails
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      category: undefined,
      subCategory: undefined,
      expirationDate: undefined,
      maxPrice: undefined,
      description: undefined,
      userImage: undefined,
    }
  );

  function onDropdownChange(
    event: React.FormEvent<HTMLDivElement>,
    option?: IDropdownOption
  ): void {
    const name: string | undefined = option?.key as string;
    const value: string | undefined = option?.text!;
    setFormInputs({ [name]: value });
  }

  function onImageChange(event: React.FormEvent<HTMLInputElement>): void {
    if (
      (event.target as HTMLInputElement).files &&
      (event.target as HTMLInputElement).files![0]
    ) {
      setFormInputs({
        userImage: URL.createObjectURL(
          (event.target as HTMLInputElement).files![0]
        ),
      });
    }
  }

  return (
    <React.Fragment>
      <Text
        block={true}
        className="Bold"
        styles={NewGroupBuyingFormStyles.headerStyle}
        variant="xLargePlus"
      >
        New suggestion for Group buying
      </Text>
      <Separator styles={{ root: { width: "15%" } }} />
      <Stack tokens={NewGroupBuyingFormStyles.verticalGapStackTokens}>
        <Dropdown
          id={categoryProp}
          onChange={onDropdownChange}
          label={"Product's Category"}
          styles={NewGroupBuyingFormStyles.dropdownStyles}
          options={Array.from(CategoriesMap.keys()).map((categoryName) => ({
            key: categoryProp,
            text: categoryName,
          }))}
        ></Dropdown>
        <Dropdown
          label={"Product's Sub Category"}
          styles={NewGroupBuyingFormStyles.dropdownStyles}
          options={
            formInputs.category
              ? Array.from(
                  CategoriesMap.get(formInputs.category)!.map(
                    (subCategory) => ({
                      key: subCategory,
                      text: subCategory,
                    })
                  )
                )
              : []
          }
        ></Dropdown>
        <TextField
          label="Maximum price"
          onGetErrorMessage={() => "Only numbers allowd"}
          styles={{ root: { width: "14%" } }}
          suffix="₪"
        />
        <DatePicker
          label="Select expiration date for the group"
          firstDayOfWeek={DayOfWeek.Sunday}
          strings={DayPickerStrings}
          placeholder="Select a date..."
          ariaLabel="Select a date"
          styles={{ root: { width: "14%" } }}
        />
        <TextField
          label="Description"
          multiline
          autoAdjustHeight
          styles={{ root: { width: "14%" } }}
        />
        <Label>{"Upload an image for reference only"}</Label>
        <input
          accept="image/*"
          onChange={onImageChange}
          id="contained-button-file"
          name="image"
          type="file"
          style={{ width: "14%" }}
        />
        <Image
          src={formInputs.userImage}
          id="target"
          width={300}
          height={300}
        />
        <Separator styles={{ root: { width: "15%" } }} />
        <Stack
          horizontal
          tokens={NewGroupBuyingFormStyles.horizontalGapStackTokens}
        >
          <DefaultButton
            text="Cancel"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            // checked={checked}
          />
          <PrimaryButton
            text="Send"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            //checked={checked}
          />
        </Stack>
      </Stack>
    </React.Fragment>
  );
};

interface NewGroupBuyDetails {
  category?: string;
  subCategory?: string;
  expirationDate?: Date;
  maxPrice?: number;
  description?: string;
  userImage?: string;
}

const categoryProp: string = "category";

const DayPickerStrings: IDatePickerStrings = {
  months: [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ],

  shortMonths: [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ],

  days: [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
  ],

  shortDays: ["S", "M", "T", "W", "T", "F", "S"],

  goToToday: "Go to today",
  prevMonthAriaLabel: "Go to previous month",
  nextMonthAriaLabel: "Go to next month",
  prevYearAriaLabel: "Go to previous year",
  nextYearAriaLabel: "Go to next year",
  closeButtonAriaLabel: "Close date picker",
  monthPickerHeaderAriaLabel: "{0}, select to change the year",
  yearPickerHeaderAriaLabel: "{0}, select to change the month",
};
