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
import { CategoriesMap } from "../HomePage/Model/Categories";
import { v4 as uuidv4 } from 'uuid';

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
    <Stack horizontalAlign={"center"}>
      <Text
        block={true}
        className="Bold"
        styles={NewGroupBuyingFormStyles.headerStyle}
        variant="xLargePlus"
      >
        New suggestion for Group buying
      </Text>
      <Stack
        styles={{ root: { width: "40%" } }}
        tokens={NewGroupBuyingFormStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        <TextField
          label="Product's name"
          styles={{ root: { width: NewGroupBuyingFormStyles.inputWidth } }}
        />
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
          styles={{ root: { width: NewGroupBuyingFormStyles.inputWidth } }}
          suffix="₪"
        />
        <DatePicker
          label="Select expiration date for the group"
          firstDayOfWeek={DayOfWeek.Sunday}
          strings={DayPickerStrings}
          placeholder="Select a date..."
          ariaLabel="Select a date"
          styles={{ root: { width: NewGroupBuyingFormStyles.inputWidth } }}
        />
        <TextField
          label="Description"
          multiline
          autoAdjustHeight
          styles={{ root: { width: NewGroupBuyingFormStyles.inputWidth } }}
        />
        <Label>{"Upload an image for reference only"}</Label>
        <input
          accept="image/*"
          onChange={onImageChange}
          id="contained-button-file"
          name="image"
          type="file"
          style={{ width: NewGroupBuyingFormStyles.inputWidth }}
        />
        {formInputs?.userImage && (
          <Image
            src={formInputs.userImage}
            id="target"
            width={"30rem"}
            height={"25rem"}
          />
        )}
        <Separator
          styles={{ root: { width: NewGroupBuyingFormStyles.inputWidth } }}
        />
        <Stack
          horizontal
          tokens={NewGroupBuyingFormStyles.horizontalGapStackTokens}
          styles={{ root: { margin: "auto" } }}
        >
          <DefaultButton
            text="Cancel"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            // checked={checked}
            href={"/"}
          />
          <PrimaryButton
            text="Send"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            //checked={checked}
            href={`/products/${uuidv4()}`}
          />
        </Stack>
      </Stack>
    </Stack>
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
